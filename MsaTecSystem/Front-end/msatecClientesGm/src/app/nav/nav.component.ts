import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SearchHelperService } from '../_shared_services/search-helper.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  searchVal: string = '';
  @Output() searchValChange: EventEmitter<string> = new EventEmitter<string>();

  constructor(private searchHelperService: SearchHelperService)
  {

  }
  
  public onSearching()
  {
    console.log('Searching ...', this.searchVal);
    //this.searchValChange.emit(this.searchVal);
    this.searchHelperService.setSearchValue(this.searchVal);
  }
}
