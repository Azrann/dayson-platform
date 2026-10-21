import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'daysonapp-admin-team-details-view',
    templateUrl: './admin-team-details-view.component.html',
    styleUrls: ['./admin-team-details-view.component.scss']
})
export class AdminTeamDetailsViewComponent implements OnInit {
    userId: string;

    constructor(
        private activatedRoute: ActivatedRoute,
    ) { }

    ngOnInit() {

        this.activatedRoute.params.subscribe((params) => {
            this.userId = params['userId'];

            if (!this.userId) {
                throw new Error(`Couldnt get proper userId from params: ${params['userId']}`);
            }
        });

    }
}
