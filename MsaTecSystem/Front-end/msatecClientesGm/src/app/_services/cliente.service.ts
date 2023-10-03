import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ClienteForLists } from '../Models/cliente';

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

  insertNovo(model: any)
  {
    const gotUrl = this.baseUrl;
  }

  excluirById(id: string)
  {
    const gotUrl = this.baseUrl;
  }
}
