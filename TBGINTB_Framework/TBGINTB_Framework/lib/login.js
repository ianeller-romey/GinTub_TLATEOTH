﻿$(document).ready(function init() {
    $('#login-button').click(function () {
        var emailAddress = $('#login-email').val();
        var password = $('#login-password').val();

        $.ajax({
            url: "gintubservices/GinTubService.svc/PlayerLogin",
            type: 'post',
            dataType: 'text',
            contentType: 'application/json',
            data: JSON.stringify({
                emailAddress: emailAddress,
                password: password
            }),
            success: function (data, status) {
                var playerLogin = JSON.parse(data);
                if (typeof (Storage) !== "undefined") {
                    sessionStorage.playerId = playerLogin.playerId;
                } else {
                    alert('Web Storage not available');
                }
                window.location = 'play.html';
            },
            error: function (request, status, error) {
                var iii = 0;
            }
        });
    });
});