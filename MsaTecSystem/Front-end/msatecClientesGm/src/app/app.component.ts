import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ClienteForLists } from './Models/cliente';
import { ClienteService } from './_services/cliente.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Msatec- Clientes';
   //clienteList: ClienteForLists[] = [];
   clienteList: any;
  constructor(private clienteService: ClienteService){

  }

  ngOnInit(): void {
    this.clienteService.getAll().subscribe({
      next: (response) => this.clienteList = response,
      error: (msgerror) => {
        console.log(msgerror);
      },
      complete: () => {
        console.log('Requested has completed!');
      }
    });
  }

  onSearchValValueChange(searchVal: string)
  {
    const updatedValue = searchVal.toLowerCase();
    console.log('parent got you!!!!!!!!!!!!!!! Updated Input Value:', updatedValue);
    //this.clienteList.filter(item.nome => item.nome.toLowerCase().includes(updatedValue));
  }
}
