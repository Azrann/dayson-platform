import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidayMonthComponent } from './holiday-month.component';

describe('HolidayMonthComponent', () => {
  let component: HolidayMonthComponent;
  let fixture: ComponentFixture<HolidayMonthComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidayMonthComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidayMonthComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
