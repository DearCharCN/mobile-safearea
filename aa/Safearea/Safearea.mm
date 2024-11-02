//
//  Safearea.m
//  Safearea
//
//  Created by 谢英伦 on 2024/8/25.
//

extern "C" {
#import <UIKit/UIKit.h>

    float getScreenHeight() {
        return [UIScreen mainScreen].bounds.size.height;
    }

    float getNotchHeight() {
        UIWindow *activeWindow = nil;

        if (@available(iOS 13.0, *)) {
            // iOS 13.0及以上版本，使用UIWindowScene
            NSSet<UIScene *> *connectedScenes = [UIApplication sharedApplication].connectedScenes;
            for (UIScene *scene in connectedScenes) {
                if (scene.activationState == UISceneActivationStateForegroundActive &&
                    [scene isKindOfClass:[UIWindowScene class]]) {
                    UIWindowScene *windowScene = (UIWindowScene *)scene;
                    for (UIWindow *window in windowScene.windows) {
                        if (window.isKeyWindow) {
                            activeWindow = window;
                            break;
                        }
                    }
                }
                if (activeWindow != nil) {
                    break;
                }
            }
        } else {
            // iOS 12及以下版本，继续使用keyWindow
            activeWindow = [UIApplication sharedApplication].delegate.window;
        }

        if (activeWindow) {
            UIEdgeInsets safeAreaInsets = activeWindow.safeAreaInsets;
            return safeAreaInsets.top > 20 ? safeAreaInsets.top : 0;
        }
        return 0;
    }

    float getNotchPercentage() {
        float screenHeight = getScreenHeight();
        float notchHeight = getNotchHeight();
        return (notchHeight / screenHeight);
    }

}
