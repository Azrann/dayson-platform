import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestedHolidayButtonComponent } from './requested-holiday-button.component';

describe('RequestedHolidayButtonComponent', () => {
  let component: RequestedHolidayButtonComponent;
  let fixture: ComponentFixture<RequestedHolidayButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestedHolidayButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestedHolidayButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
