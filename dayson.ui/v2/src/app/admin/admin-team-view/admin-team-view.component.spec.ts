import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminTeamViewComponent } from './admin-team-view.component';

describe('AdminTeamViewComponent', () => {
  let component: AdminTeamViewComponent;
  let fixture: ComponentFixture<AdminTeamViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminTeamViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminTeamViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
