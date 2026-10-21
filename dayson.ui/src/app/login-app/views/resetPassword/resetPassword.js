angular.module('login-app').directive('doResetPassword', function () {
  return {
    templateUrl: 'app/login-app/views/resetPassword/resetPassword.html',
    controller: 'resetPasswordController',
    controllerAs: 'vm'
  };
});
