import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Cliente } from '../Models/cliente';
import { ClienteService } from '../_services/cliente.service';
import {TipoTelefoneDTO} from '../Models/DTOs/tipoTelefoneDto';

@Component({
  selector: 'app-cadastro-cliente',
  templateUrl: './cadastro-cliente.component.html',
  styleUrls: ['./cadastro-cliente.component.css']
})
export class CadastroClienteComponent implements OnInit {

  @Output() cancelcadastro = new EventEmitter();
  model: Cliente = {
    id: '', 
    nome: '',
    email: '',
    dataNascimento: null, 
    telefones: [],
    isInserting: false 
  };

  tipoTelefoneOptions: TipoTelefoneDTO[] = [
    { descricao: 'Pessoal', id: 1 },
    { descricao: 'Comercial', id: 2 },
    { descricao: 'Residencial', id: 3 },
    { descricao: 'Outros', id: 4 }
  ];

  selectedTipoTelefone: number = 0;
  addingTelefoneVal: string = '';
  selectedTipoTelefoneDesc: string = '';

  constructor(private clienteService: ClienteService){}
  
  ngOnInit(): void {
    //throw new Error('Method not implemented.');
  }

  cadastrar()
  {
    this.clienteService.insertNovo(this.model).subscribe({
      next: (response: any) => {console.log(response)}
    });
  }

  cancelar()
  {
    this.model = {
      id: '', 
      nome: '',
      email: '',
      dataNascimento: null, 
      telefones: [],
      isInserting: false 
    };

    this.cancelcadastro.emit(false);
  }

  addTelefone() {
    console.log('adding phoe:', this.addingTelefoneVal);
    console.log(this.selectedTipoTelefone);
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
