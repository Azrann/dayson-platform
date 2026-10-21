export class CompanySize {
    public static sizes: CompanySize[] = [
        new CompanySize(0, 0, 5, 'Less than 5 employees'),
        new CompanySize(1, 5, 10, '5 - 10 employees'),
        new CompanySize(2, 10, 20, '10 - 20 employees'),
        new CompanySize(3, 20, 50, '20 - 50 employees'),
        new CompanySize(4, 50, 0,  'More than 50 employees')
    ];

    constructor(
        public id: number,
        public min: number,
        public max: number,
        public name: string
    ) {
    }
}

