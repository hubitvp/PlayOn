// Shared helpers: antiforgery token for AJAX calls and the notification bell.
(function () {
    const token = () =>
        document.querySelector('#antiforgery input[name="__RequestVerificationToken"]')?.value ?? '';
    window.playon = { token };

    const bell = document.getElementById('bell');
    if (!bell) return;
    const count = document.getElementById('bell-count');
    const list = document.getElementById('bell-list');

    async function load() {
        const response = await fetch(bell.dataset.url, { headers: { Accept: 'application/json' } });
        if (!response.ok) return;
        const data = await response.json(); // { unread, items: [{ title, message, link, isRead }] }
        count.textContent = data.unread;
        count.classList.toggle('d-none', data.unread === 0);
        list.replaceChildren(...data.items.map(n => {
            const li = document.createElement('li');
            const a = document.createElement('a');
            a.className = 'dropdown-item' + (n.isRead ? '' : ' fw-semibold');
            a.href = n.link ?? '#';
            a.textContent = `${n.title}: ${n.message}`;   // textContent: never inject user text as HTML
            li.append(a);
            return li;
        }));
    }

    load();
    setInterval(load, 30000);
    bell.addEventListener('shown.bs.dropdown', () =>
        fetch('/api/notifications/read', { method: 'POST', headers: { RequestVerificationToken: token() } })
            .then(load));
})();
