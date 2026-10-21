import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamUserDetailsViewComponent } from './admin-team-user-details-view.component';

describe('AdminTeamUserDetailsViewComponent', () => {
  let component: AdminTeamUserDetailsViewComponent;
  let fixture: ComponentFixture<AdminTeamUserDetailsViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamUserDetailsViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamUserDetailsViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
