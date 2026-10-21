angular.module('login-app').directive('doLogin', function () {
  return {
    templateUrl: 'app/login-app/views/login/login.html',
    controller: 'loginController',
    controllerAs: 'vm'
  };
});
