import { Component, OnInit } from '@angular/core';
import { Cliente } from '../Models/cliente';
import { ClienteService } from '../_services/cliente.service';
import {TipoTelefoneDTO} from '../Models/DTOs/tipoTelefoneDto';

@Component({
  selector: 'app-cadastro-cliente',
  templateUrl: './cadastro-cliente.component.html',
  styleUrls: ['./cadastro-cliente.component.css']
})
export class CadastroClienteComponent implements OnInit {

  model: Cliente = {
    id: '', 
    nome: '',
    email: '',
    dataNascimento: null, 
    telefones: [] 
  };

  tipoTelefoneOptions: TipoTelefoneDTO[] = [
    { descricao: 'Pessoal', id: 1 },
    { descricao: 'Comercial', id: 2 },
    { descricao: 'Residencial', id: 3 },
    { descricao: 'Outros', id: 4 }
  ];

  selectedTipoTelefone: number = 0;
  addingTelefoneVal: string = '';
  constructor(private clienteService: ClienteService){}
  
  ngOnInit(): void {
    //throw new Error('Method not implemented.');
  }

  cadastrar()
  {

  }

  cancelar()
  {

  }

  addTelefone() {
    this.model.telefones.push({ id: '', 
                                clienteId: this.model.id, 
                                numero: this.addingTelefoneVal, 
                                tipoVal: this.selectedTipoTelefone,
                                tipo: '' });
  }

  removeTelefone(index: number) {
    this.model.telefones.splice(index, 1);
  }

}
