import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamUserEditComponent } from './admin-team-user-edit.component';

describe('AdminTeamUserEditComponent', () => {
  let component: AdminTeamUserEditComponent;
  let fixture: ComponentFixture<AdminTeamUserEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamUserEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamUserEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
