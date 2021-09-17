import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  _filtroLista: string = '';
  get filtroLista(): string{
    return this._filtroLista
  }
  set filtroLista(value: string){
    this._filtroLista = value;
  }

  eventosFiltrados: any = [];
  eventos: any;
  imagemMargem = 2;
  imagemLargura = 30;
  mostrarImagem = true;

  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem;
  }


  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  getEventos() {
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.eventos = response;
    }, error => {
      console.log(error);
    });
  }
}