import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaysUserSelectorComponent } from './holidays-user-selector.component';

describe('HolidaysUserSelectorComponent', () => {
  let component: HolidaysUserSelectorComponent;
  let fixture: ComponentFixture<HolidaysUserSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidaysUserSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidaysUserSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
