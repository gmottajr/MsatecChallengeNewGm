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
    if (!this.model.nome || !this.model.email || this.model.telefones.length == 0)
    {
      alert('Prencha os dados do clientes');
      return;
    }
    
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
    const descricaoTipo = this.tipoTelefoneOptions[this.selectedTipoTelefone -1].descricao;
    console.log(descricaoTipo);
    this.model.telefones.push({ id: '', 
                                clienteId: this.model.id, 
                                numero: this.addingTelefoneVal, 
                                tipo: this.selectedTipoTelefone,
                                tipoDescricao: descricaoTipo });
  }

  removeTelefone(index: number) {
    this.model.telefones.splice(index, 1);
  }

}
