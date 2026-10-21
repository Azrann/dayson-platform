import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaysViewComponent } from './holidays-view.component';

describe('HolidaysViewComponent', () => {
  let component: HolidaysViewComponent;
  let fixture: ComponentFixture<HolidaysViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidaysViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidaysViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
