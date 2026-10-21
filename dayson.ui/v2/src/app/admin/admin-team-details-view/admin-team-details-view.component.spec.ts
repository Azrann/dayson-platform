import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamTeamDetailsViewComponent } from './admin-team-team-details-view.component';

describe('AdminTeamTeamDetailsViewComponent', () => {
  let component: AdminTeamTeamDetailsViewComponent;
  let fixture: ComponentFixture<AdminTeamTeamDetailsViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamTeamDetailsViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamTeamDetailsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
