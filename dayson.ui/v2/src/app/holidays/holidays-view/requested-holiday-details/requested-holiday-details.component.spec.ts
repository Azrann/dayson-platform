import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestedHolidayDetailsComponent } from './requested-holiday-details.component';

describe('RequestedHolidayDetailsComponent', () => {
  let component: RequestedHolidayDetailsComponent;
  let fixture: ComponentFixture<RequestedHolidayDetailsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RequestedHolidayDetailsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RequestedHolidayDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
