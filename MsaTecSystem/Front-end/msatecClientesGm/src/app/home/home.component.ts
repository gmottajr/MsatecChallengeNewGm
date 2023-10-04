import { Component, OnInit, OnDestroy } from '@angular/core';
import { ClienteForLists } from '../Models/cliente';
import { ClienteService } from '../_services/cliente.service';
import { SearchHelperService } from '../_shared_services/search-helper.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy{
  clienteList: ClienteForLists[] = [];
  clienteListAux: ClienteForLists[] = [];
  subscription: any;
  
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

    deleteCliente(clienteId: string) {
      console.log('Cliente id -- deleting:', clienteId);
      this.subscription = this.clienteService.excluirById(clienteId)
      .subscribe({
        next: (response) => {
          console.log('Cliente excluído com sucesso:', response);
          // Handle success, update UI, etc.
        },
        error: (error) => {
          console.error('Erro ao excluir cliente:', error);
          // Handle error, show error message, etc.
        }
      });
    }

    ngOnDestroy(): void {
      // Unsubscribe to avoid memory leaks when the component is destroyed
      if (this.subscription) {
        this.subscription.unsubscribe();
      }
    }

}
