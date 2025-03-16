### POP NYANG
![icon](https://img.shields.io/badge/Unity-100000?style=for-the-badge&logo=unity&logoColor=white) ![icon](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)

## 개요 📝
고양이 컨셉의 캐주얼 모바일 게임, 3 Match game

## Tech Stack ✏️
- Unity
- C#
- Visual Studio
- Sourcetree

## 기술 🔎
- 각각의 Tile과 Tile들의 정보를 가진 Board로 3 match 구현
- Await와 Async로 비동기 프로그래밍 구현

## Script로 보는 핵심 기능 📰

### Ray로 터치한 Tile의 정보 받아오기, 모든 타일을 체크할 때 까지 대기
```ruby
Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
RaycastHit2D ray = Physics2D.Raycast(touchPos, Camera.main.transform.forward);

if (ray.collider != null)
{
    Tile touchTile = ray.transform.gameObject.GetComponent<Tile>();
    if (touchTile != null && !selectedTiles.Contains(touchTile))
    {
        OnClickTile(touchTile);
    }
}
...
if (selectedTiles.Count == 2)
{
    if (Array.Exists(selectedTiles[0].checkTiles, x => x == _tile))
    {
        canControl = false;
        AudioManager.instance.PlayEffect(AudioManager.EffectType.Swap);

        await DoSwap(selectedTiles[0], selectedTiles[1]);
        await CheckAllTiles();

        if (canPop == false && popCount == 0)
        {
            AudioManager.instance.PlayEffect(AudioManager.EffectType.Fail);
            await DoSwap(selectedTiles[1], selectedTiles[0]);
        }
    }
    selectedTiles.Clear();
}
...
foreach (Row _row in rows)
{
    foreach (Tile _tile in _row.tiles)
    {
        checkedTiles.Clear();
        await CheckTile(_tile);

        if (checkedTiles.Count > 2)
        {
            canPop = true;
            popTiles.AddRange(checkedTiles);
        }
    }
}
if (canPop)
{
    popTiles = popTiles.Distinct().ToList();
    await Pop(popTiles);
    await CheckAllTiles();
}
```

터치 지점에 Ray를 쏴서 타일 정보를 받아옵니다. 선택된 타일 정보가 2개일 경우 이웃한 타일인지 체크한 후 타일 정보를 서로 교환합니다.  
모든 타일을 확인할 때 까지 기다리고 터질 수 있는 타일을 한 번에 터트립니다.

## Sample Image 🎮
<img src="https://github.com/user-attachments/assets/381a8edc-49f3-4f98-8c3f-9744ae775fba" width="270" height="480"/>  
<img src="https://github.com/user-attachments/assets/a6101dd9-e34a-445e-b674-a6c36e68ec51" width="270" height="480"/>
