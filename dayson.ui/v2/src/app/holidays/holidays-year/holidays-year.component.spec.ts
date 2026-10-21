import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidayYearComponent } from './holiday-year.component';

describe('HolidayYearComponent', () => {
  let component: HolidayYearComponent;
  let fixture: ComponentFixture<HolidayYearComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidayYearComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidayYearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
