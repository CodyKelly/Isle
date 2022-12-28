import pygame as pg
import os
import sys
import pyperclip

SCREENRECT = pg.Rect(0, 0, 1024, 1024)

main_dir = os.path.split(os.path.abspath(__file__))[0]

def load_image(file):
    """loads an image, prepares it for play"""
    file = os.path.join(main_dir, file)
    try:
        surface = pg.image.load(file)
    except pg.error:
        raise SystemExit('Could not load image "%s" %s' % (file, pg.get_error()))
    return surface.convert()

def main(winstyle=0):
    pg.init()

    fullscreen = False
    # Set the display mode
    winstyle = 0  # |FULLSCREEN
    bestdepth = pg.display.mode_ok(SCREENRECT.size, winstyle, 32)
    screen = pg.display.set_mode(SCREENRECT.size, winstyle, bestdepth)

    clock = pg.time.Clock()

    imgpath = sys.argv[1]
    tilesize = int(sys.argv[2])

    # Load images, assign to sprite classes
    # (do this before the classes are used, after screen setup)
    img = pg.transform.scale(load_image(imgpath), SCREENRECT.size)

    selecting = False
    first_select_pos = None
    select_second_pos = None
    select_width = None
    select_height = None

    pg.display.set_caption("Tile Finder")

    pg.display.flip()

    # Run our main loop whilst the player is alive.
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
                first_select_pos = (int(event.pos[0] / tilesize) * tilesize, int(event.pos[1] / tilesize) * tilesize)
            elif event.type == pg.MOUSEBUTTONUP and event.button == 1:
                selecting = False
                pyperclip.copy(f"{first_select_pos[0]}, {second_select_pos[1]}, {select_width}, {select_height}")

        screen.fill([0,0,0])
        screen.blit(img, (0, 0))

        vertical_segments = int(screen.get_height() / tilesize)
        horizontal_segments = int(screen.get_width() / tilesize)
        screen_width = screen.get_width()
        screen_height = screen.get_height()

        if selecting:
            second_select_pos = pg.mouse.get_pos()
            select_second_pos = (int(second_select_pos[0] / tilesize + 1) * tilesize, int(second_select_pos[1] / tilesize + 1) * tilesize)
            select_width = select_second_pos[0] - first_select_pos[0]
            select_height = select_second_pos[1] - first_select_pos[1]
            pg.draw.rect(screen, (255, 255, 255), pg.Rect(first_select_pos[0], first_select_pos[1], select_width, select_height))

        for x in range(horizontal_segments):
            pg.draw.line(screen, pg.Color("white"), (x * tilesize, 0), (x * tilesize, screen_height))
        for y in range(vertical_segments):
            pg.draw.line(screen, pg.Color("white"), (0, y * tilesize), (screen_width, y * tilesize))

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