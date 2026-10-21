export class CompanyCountry {
    public static countries: CompanyCountry[] = [
        new CompanyCountry('es', 'es', 'Spain'),
        new CompanyCountry('ir', 'ir', 'Ireland'),
        new CompanyCountry('gb', 'gb', 'United Kingdom'),
        new CompanyCountry('dk', 'dk', 'Denmark'),
    ];

    constructor(
        public id: string,
        public code: string,
        public name: string
    ) {
    }
}
