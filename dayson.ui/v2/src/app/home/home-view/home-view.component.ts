import { Component, OnInit } from '@angular/core';

import { IWebUserDetails } from '../../shared/data/user/web-user-details';
import { WebUserService } from '../../shared/services/web-user/web-user.service';
import { NavigationBarService } from './../../shared/navigation-bar/navigration-bar.service';

@Component({
    selector: 'daysonapp-home-view',
    templateUrl: './home-view.component.html',
    styleUrls: ['./home-view.component.scss']
})
export class HomeViewComponent implements OnInit {
    user: IWebUserDetails;

    constructor(
        private webUserService: WebUserService,
        private navigationBarService: NavigationBarService,
    ) { }

    ngOnInit() {
        this.webUserService.getCurrentUserDetails().subscribe((user: IWebUserDetails) => {
            this.user = user;

            this.setUpMenu();
        });
    }

    setUpMenu(): void {
        this.navigationBarService.setNavMenu(null, 'home-view');
    }
}
