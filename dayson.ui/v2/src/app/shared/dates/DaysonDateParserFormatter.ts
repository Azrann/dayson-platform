// import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

// function toInteger(a: any): number {
//     return parseInt(`${a}`, 10);
// }

// function isNumber(a: any): boolean {
//     return !isNaN(toInteger(a));
// }

// function padNumber(a: number, width: number): string {
//     const res = `${a}`;

//     return res.length >= width ? res : new Array(width - res.length + 1).join('0') + res;
// }

// export class DaysonDateParserFormatter extends NgbDateParserFormatter {
//     parse(value: string): NgbDateStruct {
//         if (value) {
//             const dateParts = value.trim().split('/');
//             if (dateParts.length === 1 && isNumber(dateParts[0])) {
//                 return { year: toInteger(dateParts[0]), month: null, day: null };
//             } else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
//                 return { year: toInteger(dateParts[1]), month: toInteger(dateParts[0]), day: null };
//             } else if (dateParts.length === 3 && isNumber(dateParts[0]) && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
//                 return { year: toInteger(dateParts[2]), month: toInteger(dateParts[1]), day: toInteger(dateParts[0]) };
//             }
//         }
//         return null;
//     }

//     format(date: NgbDateStruct): string {
//         return date ?
//             `${isNumber(date.day) ? padNumber(date.day, 2) : ''}/${isNumber(date.month) ? padNumber(date.month, 2) : ''}/${date.year}` :
//             '';
//     }
// }
