angular.module('login-app', ['ngRoute', 'pascalprecht.translate', 'ngMessages', 'dayson-common']);

angular.module('login-app').config(['$routeProvider', function ($routeProvider) {
  $routeProvider.
  when('/', {
    template: '<do-login></do-login>'
  }).
  when('/forgot', {
    template: '<do-forgot></do-forgot>'
  }).
  when('/register', {
    template: '<do-register></do-register>'
  }).
  when('/resetPassword/:code', {
    template: '<do-reset-password></do-reset-password>'
  }).
  otherwise({
    redirectTo: '/'
  });
}]);
