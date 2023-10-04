import { Component, OnInit } from '@angular/core';
import { ClienteForLists } from '../Models/cliente';
import { ClienteService } from '../_services/cliente.service';
import { SearchHelperService } from '../_shared_services/search-helper.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  clienteList: ClienteForLists[] = [];
  clienteListAux: ClienteForLists[] = [];
  
  constructor(private clienteService: ClienteService, 
    private searchHelperService: SearchHelperService){

  }

  ngOnInit(): void {
      this.clienteService.getAll().subscribe({
        next: (response) => {
          this.clienteList = response;
          this.clienteListAux = this.clienteList;
        },
        error: (msgerror) => {
          console.log(msgerror);
        },
        complete: () => {
          console.log('Requested has completed!');
        }
      });
      
      this.searchHelperService.searchVal$.subscribe(searchVal => {
        this.onSearchValValueChange(searchVal);
        // Handle the received search value
      });

    }

    onSearchValValueChange(searchVal: string)
    {
      this.clienteList = this.clienteListAux;
      const updatedValue = searchVal.toLowerCase();
      this.clienteList = this.clienteList.filter(clt => clt.nome.toLowerCase().includes(updatedValue));
    }

}
