import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Cliente, ClienteForLists } from '../Models/cliente';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  baseUrl: string = 'https://localhost:7159/api/';
  clienteList: any[] = [];
  constructor(private http: HttpClient) { }

  getAll(): Observable<ClienteForLists[]> {
    const gotUrl = `${this.baseUrl}cliente`;
    return this.http.get<ClienteForLists[]>(gotUrl);
  }

  getById(id: string)
  {
    const gotUrl = this.baseUrl + 'cliente';
  }

  insertNovo(model: Cliente)
  {
    model.isInserting = true;
    console.log('inerindo cliente:', model);
    const gotUrl = `${this.baseUrl}cliente`;
    return this.http.post<Cliente>(gotUrl, model).pipe(
      map(response => console.log('Inserted cliente:', response))
    );
  }

  excluirById(id: string)
  {
    const gotUrl = this.baseUrl;
  }
}
