document.addEventListener('DOMContentLoaded', function () {
    const avatar = document.getElementById('userAvatar');
    const settingsMenu = document.getElementById('userSettingsMenu');
    const closeMenuButton = document.getElementById('closeSettingsMenu');

    // Открытие меню
    avatar?.addEventListener('click', function () {
        settingsMenu.style.transform = 'translateX(0)';
    });

    // Закрытие меню
    closeMenuButton?.addEventListener('click', function () {
        settingsMenu.style.transform = 'translateX(100%)';
    });

    console.log('Header script loaded');
});