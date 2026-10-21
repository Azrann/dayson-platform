import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageFileSelectorComponent } from './image-file-selector.component';

describe('ImageFileSelectorComponent', () => {
  let component: ImageFileSelectorComponent;
  let fixture: ComponentFixture<ImageFileSelectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageFileSelectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageFileSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
