import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{
  searchVal: string = '';
  @Output() searchValChange: EventEmitter<string> = new EventEmitter<string>();

  ngOnInit(): void {
    //;
  }
  
  public onSearching()
  {
    console.log('Searching ...', this.searchVal);
    this.searchValChange.emit(this.searchVal);
  }
}
