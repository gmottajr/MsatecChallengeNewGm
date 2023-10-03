import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ClienteForLists } from './Models/cliente';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Msatec- Clientes';
   //clienteList: ClienteForLists[] = [];
   clienteList: any;
  constructor(private http: HttpClient){

  }

  ngOnInit(): void {
    this.http.get('https://localhost:7159/api/cliente').subscribe({
      next: (response) => this.clienteList = response,
      error: (msgerror) => {
        console.log(msgerror);
      },
      complete: () => {
        console.log('Requested has completed!');
      }
    });
  }
}
