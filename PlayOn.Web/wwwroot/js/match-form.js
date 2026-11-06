// Create/Edit match.
//  - courts: show only the courts that support the chosen sport (CourtSport)
//  - venue slot: it holds the date and the start time, so show them and block durations that end after closing time
//  - max players: cannot exceed the capacity of the chosen court
(function () {
    const $ = role => document.querySelector(`[data-role="${role}"]`);
    const sport = $('sport'), court = $('court'), venue = $('venue'), duration = $('duration');
    const maxPlayers = $('max-players'), info = document.getElementById('slot-info');
    const minutes = hhmm => { const [h, m] = hhmm.split(':').map(Number); return h * 60 + m; };

    function filterCourts() {
        const id = sport.value;
        court.querySelectorAll('option[data-sports]').forEach(option => {
            const ok = !id || option.dataset.sports.split(',').includes(id);
            option.hidden = !ok;
            option.disabled = !ok;
        });
        if (court.selectedOptions[0]?.disabled) court.value = '';
        limitPlayers();
    }

    function limitPlayers() {
        const capacity = court.selectedOptions[0]?.dataset.capacity;
        if (capacity) {
            maxPlayers.max = capacity;
            if (Number(maxPlayers.value) > Number(capacity)) maxPlayers.value = capacity;
        } else {
            maxPlayers.removeAttribute('max');
        }
    }

    function updateSlot() {
        const option = venue.selectedOptions[0];
        if (!option || !option.value) return;
        const available = minutes(option.dataset.closes) - minutes(option.dataset.starts);
        let best = null;
        [...duration.options].forEach(o => {
            o.disabled = Number(o.value) > available;
            if (!o.disabled) best = o;
        });
        if (duration.selectedOptions[0]?.disabled && best) duration.value = best.value;
        info.textContent = `Starts at ${option.dataset.starts}. The venue is open ${option.dataset.opens} - ` +
            `${option.dataset.closes}, so the match can last up to ${available} minutes.`;
    }

    sport.addEventListener('change', filterCourts);
    court.addEventListener('change', limitPlayers);
    venue.addEventListener('change', updateSlot);
    filterCourts();
    updateSlot();
})();
