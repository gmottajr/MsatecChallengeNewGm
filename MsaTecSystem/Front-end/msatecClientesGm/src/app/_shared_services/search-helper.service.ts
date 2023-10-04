import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SearchHelperService {
  private searchValSubject = new BehaviorSubject<string>('');
  public searchVal$ = this.searchValSubject.asObservable();
  
  constructor() { }

  setSearchValue(searchVal: string) {
    this.searchValSubject.next(searchVal);
  }
}
