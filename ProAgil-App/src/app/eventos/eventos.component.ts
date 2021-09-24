import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Evento } from '../_models/Evento';
import { EventoService } from '../_services/evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css'],
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[] = [];
  eventos: Evento[] = [];
  imagemMargem = 2;
  imagemLargura = 30;
  mostrarImagem = true;
  modalRef!: BsModalRef;
  registerForm!: FormGroup;

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
    private modalService: BsModalService
  ) {}

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  validation() {
    this.registerForm = new FormGroup({
      id: new FormControl('', [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(50),
      ]),
      local: new FormControl('', Validators.required),
      dataEvento: new FormControl('', Validators.required),
      tema: new FormControl('', Validators.required),
      qtdPessoas: new FormControl('', [
        Validators.required,
        Validators.max(120000),
      ]),
      imagemURL: new FormControl('', Validators.required),
      telefone: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
    });
  }

  salvarAlteracao() {}

  getEventos() {
    this.eventoService.getAllEvento().subscribe(
      (response) => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
