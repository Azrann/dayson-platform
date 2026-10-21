import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
// import { ControlValueAccessor } from '@angular/forms/src/directives';

@Component({
    selector: 'daysonapp-image-file-selector',
    templateUrl: './image-file-selector.component.html',
    styleUrls: ['./image-file-selector.component.scss'],
    // providers: [NG_VALUE_ACCESSOR]
})
export class ImageFileSelectorComponent implements OnInit { //implements ControlValueAccessor, OnInit {
    @Input() image: string;
    @Output() imageChange = new EventEmitter<string>();

    @Input() text: string;
    @Input() showHelp: boolean;

    fileHover: boolean;

    controlName: string;

    constructor() { }

    ngOnInit() {
        this.controlName = 'ImageFileSelectorComponent';
    }

    // writeValue(obj: any): void {
    //     throw new Error("Method not implemented.");
    // }

    // registerOnChange(fn: any): void {
    //     throw new Error("Method not implemented.");
    // }

    // registerOnTouched(fn: any): void {
    //     throw new Error("Method not implemented.");
    // }

    // setDisabledState?(isDisabled: boolean): void {
    //     throw new Error("Method not implemented.");
    // }

    fileChanged(event: Event) {
        const target: HTMLInputElement = <HTMLInputElement>event.target;
        const file = target.files.length ? target.files[0] : null;

        if (!file) { return; }

        this.changeImage(file);
    }

    changeImage(file: File) {
        const reader = new FileReader();

        reader.onload = (e: any) => {
            const dataUrl = e.target.result;
            this.image = dataUrl;
            this.imageChange.emit(dataUrl);
        };

        reader.readAsDataURL(file);
    }

    onDrop(event: DragEvent) {
        // console.log('onDrop:', event);
        this.fileHover = false;
        if (!/^image\//.test(event.dataTransfer.items[0].type)) {
            return;
        }

        event.preventDefault();

        if (event.dataTransfer.files && event.dataTransfer.files.length) {
            this.changeImage(event.dataTransfer.files[0]);
        }
    }

    onDragEnter(event: DragEvent) {
        // console.log('onDragEnter:', event);

        if (/^image\//.test(event.dataTransfer.items[0].type)) {
            this.fileHover = true;
        }
    }

    onDragLeave(event: DragEvent) {
        // console.log('onDragLeave:', event);
        this.fileHover = false;
    }

    onDragOver(event: DragEvent) {
        // console.log('onDragOver:', event);

        if (/^image\//.test(event.dataTransfer.items[0].type)) {
            event.preventDefault();
        }
    }
}
