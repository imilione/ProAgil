import { Component, OnInit, TemplateRef } from '@angular/core';
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
    this.getEventos();
  }

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
