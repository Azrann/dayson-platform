export class WeekDay {
    public static days: WeekDay[] = [
        new WeekDay(0, 'Monday'),
        new WeekDay(1, 'Tuesday'),
        new WeekDay(2, 'Wednesday'),
        new WeekDay(3, 'Thursday'),
        new WeekDay(4, 'Friday'),
        new WeekDay(5, 'Saturday'),
        new WeekDay(6, 'Sunday'),
    ];

    constructor(
        public id: number,
        public name: string
    ) {
    }
}
