import { HolidayStatus } from '../../../shared/services/holidays/data/holiday-status';
import { WebHolidayStatus } from '../../../shared/data/holiday/web-holiday-status';

export class HolidayColor extends String {

    public static requested = 'holiday-color-background-requested' as any;
    public static rejected = 'holiday-color-background-rejected' as any;
    public static accepted = 'holiday-color-background-accepted' as any;
    public static cancelled = 'holiday-color-background-cancelled' as any;

    public static backgroundPalette: HolidayColor[] = [
        'holiday-color-background-1' as any,
        'holiday-color-background-2' as any,
        'holiday-color-background-3' as any,
        'holiday-color-background-4' as any,
        'holiday-color-background-5' as any,
        'holiday-color-background-6' as any,
        'holiday-color-background-7' as any,
        'holiday-color-background-8' as any,
        'holiday-color-background-9' as any,
        'holiday-color-background-10' as any,
        'holiday-color-background-11' as any,
        'holiday-color-background-12' as any,
        'holiday-color-background-13' as any,
        'holiday-color-background-14' as any,
        'holiday-color-background-15' as any,
        'holiday-color-background-16' as any,
        'holiday-color-background-17' as any,
        'holiday-color-background-18' as any,
        'holiday-color-background-19' as any,
        'holiday-color-background-20' as any,
    ];

    public static foregroundPalette: HolidayColor[] = [
        'holiday-color-foreground-1' as any,
        'holiday-color-foreground-2' as any,
        'holiday-color-foreground-3' as any,
        'holiday-color-foreground-4' as any,
        'holiday-color-foreground-5' as any,
        'holiday-color-foreground-6' as any,
        'holiday-color-foreground-7' as any,
        'holiday-color-foreground-8' as any,
        'holiday-color-foreground-9' as any,
        'holiday-color-foreground-10' as any,
        'holiday-color-foreground-11' as any,
        'holiday-color-foreground-12' as any,
        'holiday-color-foreground-13' as any,
        'holiday-color-foreground-14' as any,
        'holiday-color-foreground-15' as any,
        'holiday-color-foreground-16' as any,
        'holiday-color-foreground-17' as any,
        'holiday-color-foreground-18' as any,
        'holiday-color-foreground-19' as any,
        'holiday-color-foreground-20' as any,
    ];

    _holidayColor: string;

    public static getByStatus(holidayStatus: WebHolidayStatus): HolidayColor {
        switch (holidayStatus) {
            case WebHolidayStatus.Accepted:
                return HolidayColor.accepted;
            case WebHolidayStatus.Rejected:
                return HolidayColor.rejected;
            case WebHolidayStatus.Canceled:
                return HolidayColor.cancelled;
            case WebHolidayStatus.Requested:
                return HolidayColor.requested;
        }

        throw new Error('holidayStatus not supported');
    }

    public static toForegroundColor(color: HolidayColor): HolidayColor {
        return color.replace('background', 'foreground') as any;
    }

    public static toBackgroundColor(color: HolidayColor): HolidayColor {
        return color.replace('foreground', 'background') as any;
    }

}
