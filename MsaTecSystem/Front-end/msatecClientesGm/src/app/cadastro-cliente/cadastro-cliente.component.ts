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
    id: '00000000-0000-0000-0000-000000000000', 
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
    if (!this.model.nome)
    {
      alert('Campo requerido: Nome.');
      return;
    }

    if (!this.model.email)
    {
      alert('Campo requerido: E-mail');
      return;
    }

    if (this.model.telefones.length == 0)
    {
      alert('Digite pelo menos um número de telefone');
      return;
    }
    
    this.clienteService.insertNovo(this.model).subscribe({
      next: (response: any) => {console.log(response)}
    });
  }

  cancelar()
  {
    this.model = {
      id: '00000000-0000-0000-0000-000000000000', 
      nome: '',
      email: '',
      dataNascimento: null, 
      telefones: [],
      isInserting: false 
    };

    this.cancelcadastro.emit(false);
  }

  addTelefone() {
    
    if(!this.addingTelefoneVal)
    {
      alert('Digite o Número do telefone');
      return;
    }

    if (!this.selectedTipoTelefone || this.selectedTipoTelefone <= 0)
    {
      alert('Selecione o tipo de telefone');
      return;
    }

    console.log('adding phoe:', this.addingTelefoneVal);
    console.log(this.selectedTipoTelefone);
    const descricaoTipo = this.tipoTelefoneOptions[this.selectedTipoTelefone -1].descricao;
    console.log(descricaoTipo);
    
    this.model.telefones.push({ id: '00000000-0000-0000-0000-000000000000', 
                                clienteId: this.model.id, 
                                numero: this.addingTelefoneVal, 
                                tipo: this.selectedTipoTelefone,
                                tipoDescricao: descricaoTipo });
  }

  removeTelefone(index: number) {
    this.model.telefones.splice(index, 1);
  }

}
