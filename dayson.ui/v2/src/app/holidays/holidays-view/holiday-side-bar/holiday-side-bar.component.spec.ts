import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HolidaySideBarComponent } from './holiday-side-bar.component';

describe('HolidaySideBarComponent', () => {
  let component: HolidaySideBarComponent;
  let fixture: ComponentFixture<HolidaySideBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HolidaySideBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HolidaySideBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
