import { DaysonPage } from './app.po';

describe('dayson App', function() {
  let page: DaysonPage;

  beforeEach(() => {
    page = new DaysonPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
