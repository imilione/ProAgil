import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  titulo = "Eventos";
  
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  evento!: Evento;
  modoSalvar = 'post';
  dataEvento!: string;
  imagemMargem = 2;
  imagemLargura = 30;
  mostrarImagem = true;
  registerForm!: FormGroup;
  idEdit!: number;
  bodyDeletarEvento = '';

  _filtroLista: string = '';

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  filtrarEventos(filtrarPor: string) {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter((evento: any) => {
      return evento.tema.toLocaleLowerCase().includes(filtrarPor);
    });
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private toastr: ToastrService
  ) {
    this.localeService.use('pt-br');
  }

  editarEvento(evento: Evento, template: any) {
    this.modoSalvar = 'put';
    this.openModal(template);
    this.evento = evento;
    this.registerForm.patchValue(evento);
  }

  novoEvento(template: any) {
    this.modoSalvar = 'post';
    this.openModal(template);
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  editModal(id: number, template: any) {
    this.registerForm.reset();
    this.idEdit = id;
    template.show();
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o evento "${evento.tema}", código "${evento.id}"`;
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: [''],
      telefone: [
        '',
        [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(11),
        ],
      ],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  salvarAlteracao(template: any) {
    if (this.modoSalvar === 'post') {
      if (this.registerForm.valid) {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento) => {
            console.log(novoEvento);
            template.hide();
            this.toastr.success('Criado com sucesso!');
            this.getEventos();
          },
          (error: any) => {
            this.toastr.error('Erro ao salvar!');
            console.log(error);
          }
        );
      }
    } else {
      this.evento = Object.assign(
        { id: this.evento.id },
        this.registerForm.value
      );
      this.eventoService.putEvento(this.evento).subscribe(
        () => {
          template.hide();
          this.toastr.success('Editado com sucesso!');
          this.getEventos();
        },
        (error: any) => {
          this.toastr.error('Erro ao salvar!');
          console.log(error);
        }
      );
    }
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEventoById(this.evento.id).subscribe(
      () => {
        template.hide();
        this.toastr.success('Deletado com sucesso!');
        this.getEventos();
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar deletar!');
        console.log(error);
      }
    );
  }

  getEventos() {
    this.eventoService.getAllEvento().subscribe(
      (response) => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
      },
      (error) => {
        this.toastr.error('Erro ao tentar carregar eventos!');
        console.log(error);
      }
    );
  }
}
