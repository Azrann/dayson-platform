import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaysTeamViewComponent } from './holidays-team-view.component';

describe('HolidaysTeamViewComponent', () => {
  let component: HolidaysTeamViewComponent;
  let fixture: ComponentFixture<HolidaysTeamViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidaysTeamViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidaysTeamViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
