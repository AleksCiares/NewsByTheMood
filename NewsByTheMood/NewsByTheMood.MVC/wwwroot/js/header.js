document.addEventListener('DOMContentLoaded', function () {
    const avatar = document.getElementById('userAvatar');
    const settingsMenu = document.getElementById('userSettingsMenu');
    const closeMenuButton = document.getElementById('closeSettingsMenu');

    avatar?.addEventListener('click', function () {
        settingsMenu.style.transform = 'translateX(0)';
    });

    closeMenuButton?.addEventListener('click', function () {
        settingsMenu.style.transform = 'translateX(100%)';
    });
});