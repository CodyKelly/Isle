import pygame as pg
import os
import sys
import pyperclip

SCREENRECT = pg.Rect(0, 0, 512, 512)

main_dir = os.path.split(os.path.abspath(__file__))[0]

def load_image(file):
    """loads an image, prepares it for play"""
    file = os.path.join(main_dir, file)
    try:
        surface = pg.image.load(file)
    except pg.error:
        raise SystemExit('Could not load image "%s" %s' % (file, pg.get_error()))
    return surface.convert()

def get_background(background_img : pg.Surface, screen : pg.Surface, tilesize):
    new_surf = background_img.copy()
    new_surf_width, new_surf_height = new_surf.get_size()
    ratio_x = screen.get_width() / new_surf_width
    ratio_y = screen.get_height() / new_surf_height
    scale_ratio = 0

    scale_ratio = ratio_y if ratio_x >= ratio_y else ratio_x

    new_surf = pg.transform.scale(new_surf, (new_surf_width * scale_ratio, new_surf_height * scale_ratio))
    new_surf_width, new_surf_height = new_surf.get_size()

    vertical_segments = int(background_img.get_height() / tilesize)
    horizontal_segments = int(background_img.get_width() / tilesize)
    scaled_tilesize = tilesize * scale_ratio

    for x in range(horizontal_segments):
        pg.draw.line(new_surf, pg.Color("white"), (x * scaled_tilesize, 0), (x * scaled_tilesize, new_surf_width))
    for y in range(vertical_segments):
        pg.draw.line(new_surf, pg.Color("white"), (0, y * scaled_tilesize), (new_surf_height, y * scaled_tilesize))

    return new_surf, scaled_tilesize
    

def main(winstyle=0):
    pg.init()

    fullscreen = False
    # Set the display mode
    winstyle = pg.RESIZABLE  # |FULLSCREEN
    bestdepth = pg.display.mode_ok(SCREENRECT.size, winstyle, 32)
    screen = pg.display.set_mode(SCREENRECT.size, winstyle, bestdepth)

    clock = pg.time.Clock()

    imgpath = sys.argv[1]
    tilesize = int(sys.argv[2])
    scaled_tilesize = tilesize

    # Load images, assign to sprite classes
    # (do this before the classes are used, after screen setup)
    img = load_image(imgpath)

    selecting = False
    first_select_pos = None
    select_second_pos = None
    select_width = None
    select_height = None

    background_dirty = True
    selection_dirty = False

    pg.display.set_caption("Tile Finder")

    pg.display.flip()

    while True:
        # get input
        for event in pg.event.get():
            if event.type == pg.QUIT:
                return
            if event.type == pg.KEYDOWN and event.key == pg.K_ESCAPE:
                return
            elif event.type == pg.KEYDOWN:
                if event.key == pg.K_f:
                    if not fullscreen:
                        print("Changing to FULLSCREEN")
                        screen_backup = screen.copy()
                        screen = pg.display.set_mode(
                            SCREENRECT.size, winstyle | pg.FULLSCREEN, bestdepth
                        )
                        screen.blit(screen_backup, (0, 0))
                    else:
                        print("Changing to windowed mode")
                        screen_backup = screen.copy()
                        screen = pg.display.set_mode(
                            SCREENRECT.size, winstyle, bestdepth
                        )
                        screen.blit(screen_backup, (0, 0))
                    pg.display.flip()
                    fullscreen = not fullscreen
            elif event.type == pg.MOUSEBUTTONDOWN and event.button == 1:
                selecting = True
                first_select_pos = (int(event.pos[0] / scaled_tilesize) * scaled_tilesize, int(event.pos[1] / scaled_tilesize) * scaled_tilesize)
            elif event.type == pg.MOUSEBUTTONUP and event.button == 1:
                selecting = False
                pyperclip.copy(f"{first_select_pos[0]}, {first_select_pos[1]}, {select_width}, {select_height}")
            elif event.type == pg.VIDEORESIZE:
                background_dirty = True

        if background_dirty:
            screen.fill([0,0,0])
            bg_surf, scaled_tilesize = get_background(img, screen, tilesize)
            screen.blit(bg_surf, (0, 0))

        if selecting:
            select_second_pos = pg.mouse.get_pos()
            select_second_pos = (int(select_second_pos[0] / scaled_tilesize + 1) * scaled_tilesize, int(select_second_pos[1] / scaled_tilesize + 1) * scaled_tilesize)
            select_width = select_second_pos[0] - first_select_pos[0]
            select_height = select_second_pos[1] - first_select_pos[1]
            pg.draw.rect(screen, (255, 255, 255), pg.Rect(first_select_pos[0], first_select_pos[1], select_width, select_height))

        pg.display.flip()

        # cap the framerate at 40fps. Also called 40HZ or 40 times per second.
        clock.tick(40)


# call the "main" function if running this script
if __name__ == "__main__":
  if len(sys.argv) != 3:
    print("args wrong")
    exit()
  main()
  pg.quit()
