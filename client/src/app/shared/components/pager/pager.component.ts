import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount;
  @Input() pageSize;
  @Output() pageChanged = new EventEmitter<number>();
  constructor() { }

  ngOnInit(): void {
  }

  onPageChanged(event) {
    this.pageChanged.emit(event.page);
  }



}
