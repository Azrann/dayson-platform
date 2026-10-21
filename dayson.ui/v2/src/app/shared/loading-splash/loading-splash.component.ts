import { Component, OnInit, Input } from '@angular/core';

@Component({
    selector: 'daysonapp-loading-splash',
    templateUrl: './loading-splash.component.html',
    styleUrls: ['./loading-splash.component.scss']
})
export class LoadingSplashComponent implements OnInit {
    @Input() fullscreen: boolean;
    @Input() transparent: boolean;
    @Input() loadingText: string;

    constructor() { }

    ngOnInit() {
    }

}
