angular.module('login-app').directive('doRegister', function () {
  return {
    templateUrl: 'app/login-app/views/register/register.html',
    controller: 'registerController',
    controllerAs: 'vm'
  };
});
