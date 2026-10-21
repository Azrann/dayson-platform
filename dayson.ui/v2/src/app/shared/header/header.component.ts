import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'daysonapp-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
    @Input() subtitle: string;

    constructor() { }

    ngOnInit() {
    }

}
