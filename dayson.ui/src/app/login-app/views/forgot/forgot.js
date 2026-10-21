angular.module('login-app').directive('doForgot', function () {
  return {
    templateUrl: 'app/login-app/views/forgot/forgot.html',
    controller: 'forgotController',
    controllerAs: 'vm'
  };
});
